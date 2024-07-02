export interface GameResponseDTO {
    resultId: number;
    blackJackId: number | null;
    rouletteId: number | null;
    banditId: number ;
    description: string;
    startDate: Date ;
    endDate: Date 
    maxBet: number;
    minBet: number;
    amount: number;
}