*****************************
******** readme.txt *********
*****************************

This file will give you basic instructions on getting the project up and going in your own environment. Once you've pulled the project down from the repo follow the following steps to make it run.


Setup the Database
-------------------
- Create a sql server database (2005 or later) and run the script in the Database folder to create the database structure. 
- Setup a user with datareader and datawriter rights.

Setup Connection String Info
-----------------------------
- In the /Code/LANUG folder you will find a sampleConnection.config file. This file will give you the basic format you will need for you connection strings. Rename sampleConnection.config to Connection.config and update the connection strings with the information for your test database.

Setup First User
-----------------
- An action has been setup to allow you to easily setup the initial user in the database, which will allow you to log into the admin area of the site. To do this, access the following URL with the info you want to use: http://localhost:9999/admin/account/createfirstuser?password=pass&username=admin (replace the localhost:9999 portion of the url with the path to your local site and replace pass and admin with the username and password you would like to use for the first user. Your passwork can be changed after setting up the user and logging in.

Acessing The Site
------------------
- After completing the steps above you should be able to run the project and view the site. To login to the admin area use the following url: http://localhost:9999/admin/account/login (replace localhost:9999 portion of the URL with the path to your local site)

Additional Setup
-----------------
- To get email service working you will need to rename the sampleSettings.config to Settings.config and update the information in that file with your smtp account information.

****************************************************************
Please feel free to contribute to this project. If I can ever be of assistance please get in touch with the info below:

Chris Yokley
cyokley@gmail.com
