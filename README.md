# Microservicio de usuarios (User)
Este microservicio hace parte de los microservicios desarrollados para la aplicación Périmé. Aquí puede encontrar más información: https://github.com/ArqSoft2020.

Desarrollado en el lenguaje C# bajo el framework ASP.NET Core, con base de datos PostgreSQL y con doble verificación para la autenticación por medio de LDAP incluido.

## Deploy 
Para desplegar este microservicio en docker use :

```
sudo docker-compose up --build
```

# Creación de usuarios

Para la creación de usuario se deben tener en cuentas las siguientes restricciones:

- El atributo *username_user* debe contener al menos 5 carácteres y máximo 16.
- El atributo *address_user* debe contener al menos 10 carácteres y máximo 30.
- El atributo *cellphone_user* debe iniciar con un 3 y tener a continuación otros 9 dígitos.
- El atributo *email_user* debe contener un nombre de cuenta, el arroba (@) y finalmente el dominio.

# Continuará...
