INSERT INTO public.rights( name, description, key)	VALUES ('administrationImport', '', 'administrationImport');

CREATE TABLE public.servicelogdescription
(
    id bigint NOT NULL,
    objecthash uuid NOT NULL,
    listid bigint NOT NULL,
    listidentifier character varying NOT NULL,
    shortcut character varying(20),
    name character varying,
    value3 character varying,
    value4 character varying,
    PRIMARY KEY (id)
);

CREATE TABLE public.servicelogtype
(
    id bigint NOT NULL,
    objecthash uuid NOT NULL,
    listid bigint NOT NULL,
    listidentifier character varying NOT NULL,
    shortcut character varying(20),
    name character varying,
    value3 character varying,
    value4 character varying,
    PRIMARY KEY (id)
);