namespace ReteCs {
    class ReteCsInterop {
        public getPosition(ref: HTMLElement): number[] {
            const centerX = ref.offsetLeft + ref.offsetWidth / 2;
            const centerY = ref.offsetTop + ref.offsetHeight / 2;
            return [centerX, centerY];
        }
    }

    export function Load(): void {
        window['ReteCsInterop'] = new ReteCsInterop();
    }
}
ReteCs.Load();