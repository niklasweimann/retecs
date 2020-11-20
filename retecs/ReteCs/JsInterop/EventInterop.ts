namespace ReteCs {
    class ReteCsInterop {
        public getPosition(ref: any): number[] {
            if(ref == null){
                console.warn("Ref was null")
                return null;
            }
            const element = ref;
            console.log(ref);
            console.log("Ref was: " + ref.current);
            window['afa'] = ref.current;
            const centerX = element.offsetLeft + element.offsetWidth / 2;
            const centerY = element.offsetTop + element.offsetHeight / 2;
            console.log("x: "+ centerX)
            console.log("y: "+ centerY)
            console.log(JSON.stringify([centerX, centerY]))
            return [centerX, centerY];
        }

        public activate(ref: any) {
            const x = ref.innerHTML;
        }

        //public registerResizeListener(dotNetObject): void{
        //    window.addEventListener('resize', () => {
        //        // @ts-ignore
        //        dotNetObject.invokeMethod('OnWindowResize');
        //    });
        //}

        public getDimensions():any {
            return {
                width: window.innerWidth,
                height: window.innerHeight
            };
        }
    }

    export function Load(): void {
        window['ReteCsInterop'] = new ReteCsInterop();
    }
}
ReteCs.Load();
