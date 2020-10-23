using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using retecs.ReteCs.Entities;
using retecs.ReteCs.Enums;
using retecs.ReteCs.View;

namespace retecs.ReteCs.core
{
    public class Emitter
    {
        #region Generic

        public delegate void WarnEventHandler(string warning, object data);

        public delegate void ErrorEventHandler(string error, object data);

        public delegate void ComponentRegisterEventHandler(Component component);

        public delegate void DestroyEventHandler();

        public event WarnEventHandler Warn;
        public event ErrorEventHandler Error;
        public event ComponentRegisterEventHandler ComponentRegister;
        public event DestroyEventHandler Destroy;

        public void OnWarn(string warn, object data) => Warn?.Invoke(warn, data);
        public void OnError(string error, object data) => Error?.Invoke(error, data);
        public void OnComponentRegister(Component component) => ComponentRegister?.Invoke(component);
        public void OnDestroy() => Destroy?.Invoke();

        #endregion

        #region Nodes

        public delegate void NodeCreateEventHandler(Node node);

        public delegate void NodeCreatedEventHandler(Node node);

        public delegate void NodeRemoveEventHandler(Node node);

        public delegate void NodeRemovedEventHandler(Node node);

        public delegate void NodeTranslateEventHandler(Node node, double x, double y);

        public delegate void NodeTranslatedEventHandler(Node node, double prevX, double prevY);

        public delegate void TranslateNodeEventHandler(Node node, double dx, double dy);

        public delegate void NodeDraggedEventHandler(Node node);

        public delegate void SelectNodeEventHandler(Node node, bool accumulate);

        public delegate void MultiSelectNodeEventHandler(Node node, bool accumulate, MouseEventArgs mouseEventArgs);

        public delegate void NodeSelectEventHandler(Node node);

        public delegate void NodeSelectedEventHandler(Node node);

        public delegate void RenderNodeEventHandler(ElementReference elementReference, Node node, object componentData,
            Action bindSocket, Action bindControl);

        public event NodeCreateEventHandler NodeCreate;
        public event NodeCreatedEventHandler NodeCreated;
        public event NodeRemoveEventHandler NodeRemove;
        public event NodeRemovedEventHandler NodeRemoved;
        public event NodeTranslateEventHandler NodeTranslate;
        public event TranslateNodeEventHandler TranslateNode;
        public event NodeTranslatedEventHandler NodeTranslated;
        public event NodeDraggedEventHandler NodeDragged;
        public event SelectNodeEventHandler SelectNode;
        public event MultiSelectNodeEventHandler MultiSelectNode;
        public event NodeSelectEventHandler NodeSelect;
        public event NodeSelectedEventHandler NodeSelected;
        public event RenderNodeEventHandler RenderNode;

        public void OnNodeCreate(Node node) => NodeCreate?.Invoke(node);
        public void OnNodeCreated(Node node) => NodeCreated?.Invoke(node);
        public void OnNodeRemove(Node node) => NodeRemove?.Invoke(node);
        public void OnNodeRemoved(Node node) => NodeRemoved?.Invoke(node);
        public void OnNodeTranslate(Node node, double x, double y) => NodeTranslate?.Invoke(node, x, y);
        public void OnTranslateNode(Node node, double dx, double dy) => TranslateNode?.Invoke(node, dx, dy);

        public void OnNodeTranslated(Node node, double prevX, double prevY) =>
            NodeTranslated?.Invoke(node, prevX, prevY);

        public void OnNodeDragged(Node node) => NodeDragged?.Invoke(node);
        public void OnSelectNode(Node node, bool accumulate) => SelectNode?.Invoke(node, accumulate);

        public void OnMultiSelectNode(Node node, bool accumulate, MouseEventArgs mouseEventArgs) =>
            MultiSelectNode?.Invoke(node, accumulate, mouseEventArgs);

        public void OnNodeSelect(Node node) => NodeSelect?.Invoke(node);
        public void OnNodeSelected(Node node) => NodeSelected?.Invoke(node);

        public void OnRenderNode(ElementReference elementreference, Node node, object componentData,
            Action bindsocket, Action bindcontrol) =>
            RenderNode?.Invoke(elementreference, node, componentData, bindsocket, bindcontrol);

        #endregion

        #region Connection

        public delegate void ConnectionCreateEventHandler(Input input, Output output);

        public delegate void ConnectionCreatedEventHandler(Connection connection);

        public delegate void ConnectionRemoveEventHandler(Connection connection);

        public delegate void ConnectionRemovedEventHandler(Connection connection);

        public event ConnectionCreateEventHandler ConnectionCreate;
        public event ConnectionCreatedEventHandler ConnectionCreated;
        public event ConnectionRemoveEventHandler ConnectionRemove;
        public event ConnectionRemovedEventHandler ConnectionRemoved;
        public void OnConnectionCreate(Input input, Output output) => ConnectionCreate?.Invoke(input, output);
        public void OnConnectionCreated(Connection connection) => ConnectionCreated?.Invoke(connection);
        public void OnConnectionRemove(Connection connection) => ConnectionRemove?.Invoke(connection);
        public void OnConnectionRemoved(Connection connection) => ConnectionRemoved?.Invoke(connection);

        #endregion

        #region Socket

        public delegate void RenderSocketEventHandler(ElementReference elementReference, Socket socket,
            Input input = null, Output output = null);

        public event RenderSocketEventHandler RenderSocket;

        public void OnRenderSocket(ElementReference elementreference, Socket socket, Input input = null, Output output = null) =>
            RenderSocket?.Invoke(elementreference, socket, input, output);

        #endregion

        #region Control

        public delegate void RenderControlEventHandler(ElementReference elementReference, Control control);

        public event RenderControlEventHandler RenderControl;

        public void OnRenderControl(ElementReference elementReference, Control control) =>
            RenderControl?.Invoke(elementReference, control);

        #endregion

        #region Connection

        public delegate void RenderConnectionEventHandler(ElementReference elementReference, Connection connection,
            List<int> points);

        public delegate void UpdateConnectionEventHandler(ElementReference elementReference, Connection connection,
            List<int> points);

        public event RenderConnectionEventHandler RenderConnection;
        public event UpdateConnectionEventHandler UpdateConnection;

        // TODO: Add dataType Point
        public void OnRenderConnection(ElementReference elementReference, Connection connection, List<int> points) =>
            RenderConnection?.Invoke(elementReference, connection, points);

        public void OnUpdateConnection(ElementReference elementReference, Connection connection, List<int> points) =>
            UpdateConnection?.Invoke(elementReference, connection, points);

        #endregion

        #region Editor

        public delegate void KeyDownEventHandler(KeyboardEventArgs keyboardEventArgs);

        public delegate void KeyUpEventHandler(KeyboardEventArgs keyboardEventArgs);

        public delegate void TranslateEventHandler(Transform transform, int x, int y);

        public delegate void TranslatedEventHandler();

        public delegate void ZoomEventHandler(Transform transform, int zoom, ZoomSource zoomSource);

        public delegate void ZoomedEventHandler(ZoomSource zoomSource);

        public delegate void ClickEventHandler(MouseEventArgs mouseEventArgs, ElementReference container);

        public delegate void MousemoveEventHandler(Mouse mouse);

        public delegate void ContextMenuEventHandler(MouseEventArgs mouseEventArgs, EditorView editorView = null,
            Node node = null);

        public delegate void ImportEventHandler(Data data);

        public delegate void ExportEventHandler(Data data);

        public delegate void ProcessEventHandler();

        public delegate void ClearEventHandler();


        public event KeyDownEventHandler KeyDown;
        public event KeyUpEventHandler KeyUp;
        public event TranslateEventHandler Translate;
        public event TranslatedEventHandler Translated;
        public event ZoomEventHandler Zoom;
        public event ZoomedEventHandler Zoomed;
        public event ClickEventHandler Click;
        public event MousemoveEventHandler MouseMove;
        public event ContextMenuEventHandler ContextMenu;
        public event ImportEventHandler Import;
        public event ExportEventHandler Export;
        public event ProcessEventHandler Process;
        public event ClearEventHandler Clear;

        public void OnKeyDown(KeyboardEventArgs keyboardEventArgs) => KeyDown?.Invoke(keyboardEventArgs);
        public void OnKeyUp(KeyboardEventArgs keyboardEventArgs) => KeyUp?.Invoke(keyboardEventArgs);
        public void OnTranslate(Transform transform, int x, int y) => Translate?.Invoke(transform, x, y);
        public void OnTranslated() => Translated?.Invoke();

        public void OnZoom(Transform transform, int zoom, ZoomSource zoomSource) =>
            Zoom?.Invoke(transform, zoom, zoomSource);

        public void OnZoomed(ZoomSource zoomSource) => Zoomed?.Invoke(zoomSource);

        public void OnClick(MouseEventArgs mouseEventArgs, ElementReference container) =>
            Click?.Invoke(mouseEventArgs, container);

        public void OnMouseMove(Mouse mouse) => MouseMove?.Invoke(mouse);

        public void OnContextMenu(MouseEventArgs mouseEventArgs, EditorView editorView = null,
            Node node = null) => ContextMenu?.Invoke(mouseEventArgs, editorView, node);

        public void OnImport(Data data) => Import?.Invoke(data);
        public void OnExport(Data data) => Export?.Invoke(data);
        public void OnProcess() => Process?.Invoke();
        public void OnClear() => Clear?.Invoke();

        #endregion
    }
}