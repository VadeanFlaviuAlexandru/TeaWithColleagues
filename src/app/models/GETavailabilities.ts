export interface GETavailabilities{
    idAvailability: number,
    idUser: number,
    idParticipant: number,
    location: string;
    type: number;
    startDate: string;
    apiUser: {
        idUser: number,
        name: string,
        surname: string,
        phoneNumber: string,
        mailAddress: string
    }
}