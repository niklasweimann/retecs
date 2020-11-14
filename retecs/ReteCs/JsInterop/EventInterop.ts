namespace ReteCs {
    class ReteCsInterop {
        public getPosition(ref: HTMLElement): number[] {
            const centerX = ref.offsetLeft + ref.offsetWidth / 2;
            const centerY = ref.offsetTop + ref.offsetHeight / 2;
            return [centerX, centerY];
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
