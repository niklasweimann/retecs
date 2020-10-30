namespace ReteCs {
    class ReteCsInterop {
    }

    export function Load(): void {
        window['ReteCsInterop'] = new ReteCsInterop();
    }
}
ReteCs.Load();