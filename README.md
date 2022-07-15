# There Will Be Mud

This is a simple ASP.NET Core web application written in C#. You selet a location (city/state and country) and it will tell you if it will be muddy in three days.

The public API provided by https://openweathermap.org is used for weather forecast data.

It will be muddy if the weather forecast predicts some rain or snow precipitation and a max temperature greater than zero on a given day.

## How to run the application using docker

1. Clone the repository

`git clone https://github.com/jimgetty/there-will-be-mud.git`

2. Change to root directory

`cd there-will-be-mud`

3. Build the docker image

`docker build -t "therewillbemud:latest" .`

4. Run the docker image

`docker run -dp 5002:5002 therewillbemud:latest`

5. Navigate to http://localhost:5002 in your browser

## How to run unit tests using docker

1. Build the docker image (in the same root directory as above)

`docker build -f "Dockerfile.tests" -t "therewillbemud_tests:latest" .`

2. Run the docker image

`docker run therewillbemud_tests:latest`
