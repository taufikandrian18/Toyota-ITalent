export class ChecklistForm {
    text: string;
    check: boolean;
    score: number;
    point: number;
    choiceNumber: number;
    timePointId: number;

    constructor() {
        this.text = "";
        this.check = false;
        this.score = 0;
        this.point = 0;
        this.choiceNumber = 0;
        this.timePointId = 0;
    }
}