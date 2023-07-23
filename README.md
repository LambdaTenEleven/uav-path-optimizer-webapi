# UAV Path Optimization

## Overview

The UAV Path Optimization project is an API-based solution that focuses on solving optimization problems related to UAV (Unmanned Aerial Vehicle) path planning. The primary goal of the project is to provide efficient routing solutions for multiple UAVs, addressing both the Vehicle Routing Problem (VRP) and the Traveling Salesman Problem (TSP).

## Features

- [x] API endpoint for solving VRP and TSP problems
- [ ] API endpoint for creating schedules for UAVs
- [ ] User authorization and authentication endpoints
- [ ] Angular application for interacting with the API

## Architecture

The project is built following the [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) principles. The main components of the architecture are:
* **[Domain](src/UavPathOptimization.Domain)** - contains the domain entities, contracts, events, interfaces and errors
* **[WebApi](src/UavPathOptimization.WebAPI)** - presentation layer, contains the API controllers and the API models
* **[Application](src/UavPathOptimization.Application)** - contains the application logic and use cases
* **[Infrastructure](src/UavPathOptimization.Infrastructure)** - contains the implementation of the application interfaces and the external dependencies

## Technologies used

TODO Describe the technologies used in the project

## Installation

TODO Describe the installation process

## Configuration

TODO Describe the configuration process

## Usage

TODO Describe the usage of the project

## API Documentation

The documentation is available here: [Docs]()

## Testing

TODO Describe the testing process

## Contributing

TODO Describe the contributing