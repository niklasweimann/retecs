using System;
using System.Net.Sockets;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using retecs.ReteCs.Entities;
using retecs.ReteCs.Enums;
using retecs.Shared;

namespace retecs.ReteCs.core
{
    public class Emitter
    {
        #region Internal
        
        #region Generic

        public delegate void WarnEventHandler(string warning, object data);

        public delegate void ErrorEventHandler(string error, object data);

        public delegate void ComponentRegisterEventHandler(Component component);

        public delegate void DestroyEventHandler();

        public event WarnEventHandler Warn;
        public event ErrorEventHandler Error;
        public event ComponentRegisterEventHandler ComponentRegister;
        public event DestroyEventHandler Destroy;

        public void OnWarn(string warn, object data = null) => Warn?.Invoke(warn, data);
        public void OnError(string error, object data = null) => Error?.Invoke(error, data);
        public void OnComponentRegister(Component component) => ComponentRegister?.Invoke(component);
        public void OnDestroy() => Destroy?.Invoke();

        #endregion

        #region Nodes

        public delegate void NodeCreateEventHandler(Node node);

        public delegate void NodeCreatedEventHandler(Node node);

        public delegate void NodeRemoveEventHandler(Node node);

        public delegate void NodeRemovedEventHandler(Node node);

        public delegate void NodeTranslateEventHandler(Node node, double x, double y);

        public delegate void NodeTranslatedEventHandler(Node node, Point prevPoint);

        public delegate void TranslateNodeEventHandler(Node node, Point point);

        public delegate void NodeDraggedEventHandler(Node node);

        public delegate void SelectNodeEventHandler(Node node, bool accumulate);

        public delegate void MultiSelectNodeEventHandler(Node node, bool accumulate, MouseEventArgs mouseEventArgs);

        public delegate void NodeSelectEventHandler(Node node);

        public delegate void NodeSelectedEventHandler(Node node);

        public delegate void RenderNodeEventHandler(ElementReference elementReference, Node node, object componentData,
            Action<ElementReference, string, Io> bindSocket, Action<ElementReference, Control> bindControl);

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
        public void OnTranslateNode(Node node, Point point) => TranslateNode?.Invoke(node, point);

        public void OnNodeTranslated(Node node, Point prevPoint) =>
            NodeTranslated?.Invoke(node, prevPoint);

        public void OnNodeDragged(Node node) => NodeDragged?.Invoke(node);
        public void OnSelectNode(Node node, bool accumulate) => SelectNode?.Invoke(node, accumulate);

        public void OnMultiSelectNode(Node node, bool accumulate, MouseEventArgs mouseEventArgs) =>
            MultiSelectNode?.Invoke(node, accumulate, mouseEventArgs);

        public void OnNodeSelect(Node node) => NodeSelect?.Invoke(node);
        public void OnNodeSelected(Node node) => NodeSelected?.Invoke(node);

        public void OnRenderNode(ElementReference elementreference, Node node, object componentData,
            Action<ElementReference, string, Io> bindsocket, Action<ElementReference, Control> bindcontrol) =>
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

        public delegate void RenderSocketEventHandler(Socket socket,Io io);

        public event RenderSocketEventHandler RenderSocket;

        public void OnRenderSocket(Socket socket, Io io) =>
            RenderSocket?.Invoke(socket, io);

        #endregion

        #region Control

        public delegate void RenderControlEventHandler(Control control);

        public event RenderControlEventHandler RenderControl;

        public void OnRenderControl(Control control) =>
            RenderControl?.Invoke(control);

        #endregion

        #region Connection

        public delegate void RenderConnectionEventHandler(ElementReference elementReference, Connection connection,
            (Point, Point) points);

        public delegate void UpdateConnectionEventHandler(ElementReference elementReference, Connection connection,
            (Point, Point) points);

        public event RenderConnectionEventHandler RenderConnection;
        public event UpdateConnectionEventHandler UpdateConnection;

        public void OnRenderConnection(ElementReference elementReference, Connection connection, (Point, Point) points) =>
            RenderConnection?.Invoke(elementReference, connection, points);

        public void OnUpdateConnection(ElementReference elementReference, Connection connection, (Point, Point) points) =>
            UpdateConnection?.Invoke(elementReference, connection, points);

        #endregion

        #region Editor

        public delegate void KeyDownEventHandler(KeyboardEventArgs keyboardEventArgs);

        public delegate void KeyUpEventHandler(KeyboardEventArgs keyboardEventArgs);

        public delegate void TranslateEventHandler(Transform transform, double x, double y);

        public delegate void TranslatedEventHandler();

        public delegate void ZoomEventHandler(Transform transform, double zoom, ZoomSource zoomSource);

        public delegate void ZoomedEventHandler(ZoomSource zoomSource);

        public delegate void ClickEventHandler(MouseEventArgs mouseEventArgs, ElementReference container);

        public delegate void MousemoveEventHandler(Mouse mouse);

        public delegate void ContextMenuEventHandler(MouseEventArgs mouseEventArgs, ReteEditor editorView = null,
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
        public void OnTranslate(Transform transform, double x, double y) => Translate?.Invoke(transform, x, y);
        public void OnTranslated() => Translated?.Invoke();

        public void OnZoom(Transform transform, double zoom, ZoomSource zoomSource) =>
            Zoom?.Invoke(transform, zoom, zoomSource);

        public void OnZoomed(ZoomSource zoomSource) => Zoomed?.Invoke(zoomSource);

        public void OnClick(MouseEventArgs mouseEventArgs, ElementReference container) =>
            Click?.Invoke(mouseEventArgs, container);

        public void OnMouseMove(Mouse mouse) => MouseMove?.Invoke(mouse);

        public void OnContextMenu(MouseEventArgs mouseEventArgs, ReteEditor editorView = null,
            Node node = null) => ContextMenu?.Invoke(mouseEventArgs, editorView, node);

        public void OnImport(Data data) => Import?.Invoke(data);
        public void OnExport(Data data) => Export?.Invoke(data);
        public void OnProcess() => Process?.Invoke();
        public void OnClear() => Clear?.Invoke();

        #endregion
        #endregion

        #region UiEvents

        #region Window
        public delegate void ResizeWindowEventHandler();
        public delegate void KeyDownWindowEventHandler(KeyboardEventArgs keyboardEventArgs);
        public delegate void KeyUpWindowEventHandler(KeyboardEventArgs keyboardEventArgs);
        public delegate void MouseMoveWindowEventHandler(MouseEventArgs mouseEventArgs);
        public delegate void MouseUpWindowEventHandler(MouseEventArgs mouseEventArgs);
        public delegate void ContextMenuWindowEventHandler(MouseEventArgs mouseEventArgs);

        public delegate void MouseDownWindowEventHandler(MouseEventArgs mouseEventArgs);

        public event ResizeWindowEventHandler WindowResize;
        public event KeyDownWindowEventHandler WindowKeyDown;
        public event KeyUpWindowEventHandler WindowKeyUp;
        public event MouseMoveWindowEventHandler WindowMouseMove;
        public event MouseUpWindowEventHandler WindowMouseUp;
        public event MouseDownWindowEventHandler WindowMouseDown;
        public event ContextMenuWindowEventHandler WindowContextMenu;

        public void OnWindowMouseDown(MouseEventArgs mouseEventArgs) => WindowMouseDown?.Invoke(mouseEventArgs);
        public void OnWindowResize() =>
            WindowResize?.Invoke();
        public void OnWindowContextMenu(MouseEventArgs mouseEventArgs) => WindowContextMenu?.Invoke(mouseEventArgs);
        public void OnWindowKeyDown(KeyboardEventArgs keyboardEventArgs) =>
            WindowKeyDown?.Invoke(keyboardEventArgs);
        public void OnWindowKeyUp(KeyboardEventArgs keyboardEventArgs) =>
            WindowKeyUp?.Invoke(keyboardEventArgs);
        public void OnWindowMouseMove(MouseEventArgs mouseEventArgs) =>
            WindowMouseMove?.Invoke(mouseEventArgs);
        public void OnWindowMouseUp(MouseEventArgs mouseEventArgs) =>
            WindowMouseUp?.Invoke(mouseEventArgs);
        
        #endregion
        #endregion
    }
}