export class Sort {
    sort: boolean;
    sortup: boolean;
    sortdown: boolean;

    constructor() {
        this.sort = true;
        this.sortup = false;
        this.sortdown = false;
    }

    public sorting() {
        if (this.sort == true) {
            this.sort = false;
            this.sortup = true;
            return;
        }

        if (this.sortup == true) {
            this.sortup = false;
            this.sortdown = true;
            return;
        }

        if (this.sortdown == true) {
            this.sortdown = false;
            this.sort = true;
            return;
        }
    }

    public reset() {
        this.sort = true;
        this.sortup = false;
        this.sortdown = false;
    }
};