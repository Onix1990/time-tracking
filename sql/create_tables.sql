CREATE TABLE users
(
    id         bigint       NOT NULL GENERATED ALWAYS AS IDENTITY,
    first_name varchar(100) NOT NULL,
    last_name  varchar(100) NOT NULL,
    patronymic varchar(100) NOT NULL,
    email      varchar(100) NOT NULL UNIQUE,
    CONSTRAINT users_pkey PRIMARY KEY (id)
);

CREATE TABLE audits
(
    id          bigint       NOT NULL GENERATED ALWAYS AS IDENTITY,
    hours       smallint     NOT NULL,
    date        date         NOT NULL,
    description varchar(255) NOT NULL,
    user_id     bigint       NOT NULL,
    CHECK (hours >= 0 AND hours <= 24),
    CONSTRAINT audits_pkey PRIMARY KEY (id),
    CONSTRAINT audits_fk0 FOREIGN KEY (user_id)
        REFERENCES users (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);