interface ISequence {
    text: string;
    point: number;
    score: number;
    timePointId: number;
}

interface ISequenceContainer {
    question: string;
    choice: ISequence[];
    answer: string;
}