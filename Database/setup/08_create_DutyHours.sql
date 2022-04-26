CREATE TABLE public.dutyhours
(
    ident uuid NOT NULL,
    signInBookingIdent uuid NOT NULL,
    signOutBookingIdent uuid NOT NULL
);

CREATE TABLE public.dutyhoursbookings
(
    ident uuid NOT NULL,
    userident uuid NOT NULL,
    creatorident uuid NOT NULL,
    bookingtime timestamp with time zone NOT NULL,
    issignedin boolean NOT NULL
);

INSERT INTO public.rights( name, description, key)	VALUES ('dutyHours', '', 'dutyHours');
INSERT INTO public.rights( name, description, key)	VALUES ('dutyHoursDisplaySelf', '', 'dutyHoursDisplaySelf');
INSERT INTO public.rights( name, description, key)	VALUES ('dutyHoursDisplayAll', '', 'dutyHoursDisplayAll');
INSERT INTO public.rights( name, description, key)	VALUES ('dutyHoursCreateBooking', '', 'dutyHoursCreateBooking');
INSERT INTO public.rights( name, description, key)	VALUES ('dutyHoursEditBooking', '', 'dutyHoursEditBooking');
INSERT INTO public.rights( name, description, key)	VALUES ('dutyHoursGetUserByChipId', '', 'dutyHoursGetUserByChipId');
