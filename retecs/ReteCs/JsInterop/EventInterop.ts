namespace ReteCs {
    class ReteCsInterop {
        public addEventListener(container: HTMLElement, eventName: string, callback: string, objectReference: any) {
            console.log("Add listener to " + (container.innerHTML) + "," + eventName)
            container.addEventListener(eventName, event => this.HandleEvent(event, callback, objectReference));
        }
        
        public addWindowEventListener(event: string, callback: string, objectReference: any){
            window.addEventListener(event, event => this.HandleEvent(event, callback, objectReference))
        }
        
        private HandleEvent(event: Event, callback: string, objectReference: any){
            this.returnEventListener(callback, objectReference, event)
        }
        
        //public removeEventListener(container: HTMLElement, event: string, callback: string, objectReference: any){
        //    container.removeEventListener(event, event => this.HandleEvent(event, callback, objectReference));
        //}
        //
        //public removeWindowEventListener(event: string, callback: string, objectReference: any){
        //    window.removeEventListener(event, event => this.HandleEvent(event, callback, objectReference));
        //}

        public returnEventListener(callback: string, objectReference: any, event: Event) {
            // @ts-ignore
            objectReference.invokeMethodAsync(
                "ReturnEventCallback",
                callback,
                this.serializeEvent(event));
        }
        
        public appendChild(parent: HTMLElement, child: HTMLElement){
            parent.appendChild(child);
        }
        
        public removeChild(parent: HTMLElement, child: HTMLElement){
            parent.removeChild(child);
        }

         private serializeEvent(e) {
            if (e) {
                return {
                    altKey: e.altKey,
                    button: e.button,
                    buttons: e.buttons,
                    clientX: e.clientX,
                    clientY: e.clientY,
                    ctrlKey: e.ctrlKey,
                    metaKey: e.metaKey,
                    movementX: e.movementX,
                    movementY: e.movementY,
                    offsetX: e.offsetX,
                    offsetY: e.offsetY,
                    pageX: e.pageX,
                    pageY: e.pageY,
                    screenX: e.screenX,
                    screenY: e.screenY,
                    shiftKey: e.shiftKey
                };
            }
        };
    }

    export function Load(): void {
        window['ReteCsInterop'] = new ReteCsInterop();
    }
}
ReteCs.Load();