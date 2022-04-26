import { User } from "./user";

export type DutyHour = {
    ident: { ident: string };
    customerId: number;
    signInBookingIdent: { ident: string };
    signOutBookingIdent: { ident: string };
    signInBooking: DutyHoursBooking;
    signOutBooking: DutyHoursBooking;
};

export type DutyHoursBooking = {
    ident: { ident: string };
    customerId: number;
    bookingTime: Date;
    isSignedIn: boolean;
    userIdent: { ident: string };
    user: User;
    creatorIdent: { ident: string };
    creator: User;
};
