
\connect perime-user-db

  CREATE TABLE "Users"
(
  id_user integer NULL GENERATED BY DEFAULT AS IDENTITY,
  username_user VARCHAR(15),
  passhash_user TEXT,
  address_user VARCHAR(30),
  cellphone_user CHAR(10) NOT NULL,
  email_user VARCHAR(30),
  CONSTRAINT "PK_Users" PRIMARY KEY (id_user)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE "Users"
  OWNER TO postgresuser;