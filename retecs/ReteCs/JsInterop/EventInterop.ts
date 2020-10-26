namespace ReteCs {
    class ReteCsInterop {
        public addEventListener(container: HTMLElement, eventName: string, callback: string, objectReference: any) {
            console.log("Add listener to " + container + "," + eventName)
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
                'retecs.ReteCs.JsInterop.ReturnEventCallback',
                "ReturnEventCallback",
                callback,
                event);
        }
        
        public appendChild(parent: HTMLElement, child: HTMLElement){
            parent.appendChild(child);
        }
        
        public removeChild(parent: HTMLElement, child: HTMLElement){
            parent.removeChild(child);
        }

        public createElement(element: HTMLElement, objectReference:any) {
            // create a new div element 
            const newDiv = document.createElement("div");

            // and give it some content 
            const newContent = document.createTextNode("Hi there and greetings!");

            // add the text node to the newly created div
            var ref =  element.appendChild(newContent);
            
            objectReference.invokeMethodAsync(
                'ReturnRefCallback',
                ref);
        } 
    }

    export function Load(): void {
        window['ReteCsInterop'] = new ReteCsInterop();
    }
}
ReteCs.Load();