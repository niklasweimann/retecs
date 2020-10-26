using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;
using retecs.ReteCs.Enums;

namespace retecs.ReteCs.Engine
{
    public class Engine : Context
    {
        public List<object> Args { get; set; }
        public object Data { get; set; }
        public State State { get; set; } = State.Available;
        public Action OnAbort { get; set; }

        public Engine(string id, Emitter emitter): base(id, emitter)
        {
        }

        public Engine Clone()
        {
            var engine = new Engine(Id, Emitter);
            foreach (var component in Components)
            {
                engine.Register(component.Value);
            }

            return engine;
        }

        public string ThrowError(string message, object data)
        {
            Abort();
            Emitter.OnWarn(message, data);
            ProcessDone();
            return "error";
        }

        private bool ProcessStart()
        {
            if (State == State.Available)
            {
                State = State.Processed;
                return true;
            }

            if (State == State.Abort)
            {
                return false;
            }
            
            Console.WriteLine("The process is busy and has not been restarted. Use abort() to force it to complete");
            return false;
        }

        private bool ProcessDone()
        {
            var success = State != State.Abort;
            State = State.Available;
            if (success)
            {
                return true;
            }
            OnAbort.Invoke();
            OnAbort = () => { };

            return false;
        }

        public void Abort()
        {
            switch (State)
            {
                case State.Processed:
                    State = State.Abort;
                    //OnAbort = ret;
                    break;
                case State.Abort:
                    OnAbort.Invoke();
                    //OnAbort = ret; 
                    break;
                default:
                    //ret();
                    break;
            }
        }

        private void Lock(EngineNode node)
        {
            node.UnlockPool ??= new List<Action>();
            if (node.Busy && node.OutputData != null)
            {
                //node.UnlockPool.Add(res);
            }
            else
            {
                //res();
            }

            node.Busy = false;
        }

        public void Unlock(EngineNode node)
        {
            node.UnlockPool.ForEach(x => x());
            node.UnlockPool = new List<Action>();
            node.Busy = false;
        }

        private Dictionary<string, List<WorkerOutput>> ExtractInputData(NodeData nodeData)
        {
            var obj = new Dictionary<string, List<WorkerOutput>>();
            foreach (var key in nodeData.Inputs.Keys)
            {
                var input = nodeData.Inputs[key];
                var connections = input.Connections;
                var connData = new List<WorkerOutput>();
                foreach (var connection in connections)
                {
                    var prevNode = (Data as Data)?.Nodes[connection.Node];
                    var outputs = ProcessNode(prevNode as EngineNode);
                    if (outputs == null)
                    {
                        Abort();
                    }
                    else
                    {
                        connData.Add(outputs[connection.Output]);
                    }
                }
                
                obj[key] = connData;
            }

            return obj;
        }

        private Dictionary<string, WorkerOutput> ProcessWorker(NodeData nodeData)
        {
            var inputData = ExtractInputData(nodeData);
            var component = Components[nodeData.Name];
            var outputData = new Dictionary<string, WorkerOutput>();
            try
            {
                component.Worker(nodeData, inputData, outputData, Args);
            }
            catch (Exception e)
            {
                Abort();
                Emitter.OnWarn(e.Message, e);
            }

            return outputData;
        }

        private Dictionary<string, WorkerOutput> ProcessNode(EngineNode node)
        {
            if (State == State.Abort || node == null)
            {
                return null;
            }
            
            Lock(node);

            if (node.OutputData == null)
            {
                node.OutputData = ProcessWorker(node);
            }
            
            Unlock(node);
            return node.OutputData;
        }

        private object ForwardProcess(NodeData node)
        {
            if (State == State.Abort)
                return null;
            var res = new List<NodeData>();
            foreach (var nextNode in node.Outputs.Values.SelectMany(output =>
                output.Connections.Select(connection => (Data as Data)?.Nodes[connection.Node])))
            {
                ProcessNode(nextNode as EngineNode);
                ForwardProcess(nextNode);
                res.Add(nextNode);
            }

            return res;
        }

        public Data Copy(Data data)
        {
            return JsonSerializer.Deserialize<Data>(JsonSerializer.Serialize(data));
        }

        public bool Validate(Data data)
        {
            var (success, message) = Validator.Validate(Id, data);
            var recursion = new Recursion(data.Nodes);
            if (!success)
            {
                throw new Exception(message);
            }

            var recurrentNode = recursion.Detect();
            if (recurrentNode != null)
            {
                throw new Exception("Recursion detected");
            }

            return true;
        }

        public void ProcessStartNode(string id)
        {
            var startNode = (Data as Data)?.Nodes[id];
            if (startNode == null)
            {
                throw new Exception("Node with such id not found");
            }

            ProcessNode(startNode as EngineNode);
            ForwardProcess(startNode);
        }

        private void ProcessUnreachable()
        {
            var data = Data as Data;
            foreach (var node in data.Nodes.Keys)
            {
                var currNode = data.Nodes[node] as EngineNode;
                if (currNode?.OutputData == null)
                {
                    ProcessNode(currNode);
                    ForwardProcess(currNode);
                }
            }
        }

        public string ProcessData(Data data, string startId = null, params object[] args)
        {
            if (!ProcessStart() || !Validate(data))
            {
                return null;
            }

            Data = data;
            Args = args.ToList();
            ProcessStartNode(startId);
            ProcessUnreachable();
            return ProcessDone() ? "sucess" : "aborted";
        }
    }
}