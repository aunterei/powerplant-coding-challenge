# Powerplant Coding Challenge

<p align="center">
    <img style="width:200px; height: auto;" src="https://raw.githubusercontent.com/aunterei/powerplant-coding-challenge/master/docs/assets/logo.png" alt="Engie logo">
</p>

<br>

| Programming language | Framework |
| -------------------- | --------- |
| C#                   | dotnet    |

This is my submission for the [power plan coding challenge](https://github.com/gem-spaas/powerplant-coding-challenge) by engie. As stated :

> The goal of this coding challenge is to calculate how much power each of a multitude of different powerplants need to produce (a.k.a. the production-plan) when the load is given and taking into account the cost of the underlying energy sources (gas, kerosine) and the Pmin and Pmax of each powerplant.

The end product is a REST API exposing an endpoint ```/productionplan``` that accepts a POST with a payload similar to:

```json
{
  "load": 480,
  "fuels": {
    "gas(euro/MWh)": 13.4,
    "kerosine(euro/MWh)": 50.8,
    "co2(euro/ton)": 20,
    "wind(%)": 60
  },
  "powerplants": [
    {
      "name": "gasfiredbig1",
      "type": "gasfired",
      "efficiency": 0.53,
      "pmin": 100,
      "pmax": 460
    },
    {
      "name": "gasfiredsomewhatsmaller",
      "type": "gasfired",
      "efficiency": 0.37,
      "pmin": 40,
      "pmax": 210
    },
    {
      "name": "tj1",
      "type": "turbojet",
      "efficiency": 0.3,
      "pmin": 0,
      "pmax": 16
    },
    {
      "name": "windpark1",
      "type": "windturbine",
      "efficiency": 1,
      "pmin": 0,
      "pmax": 150
    }
  ]
}
```

and returns:

```json
[
  {
    "name": "gasfiredbig1",
    "p": 390
  },
  {
    "name": "windpark1",
    "p": 90
  },
  {
    "name": "gasfiredsomewhatsmaller",
    "p": 0
  },
  {
    "name": "tj1",
    "p": 0
  }
]
```

<br>

## Launch the api

To launch the api, follow these steps:

* Open the solution with VisualStudio
* Press Ctrl+F5 to run without a debugger (you might need to accept the ssl certificate)

The app should start on the swagger page where you can find all information on the models and the post method. From there, the api is running and you can call it via swagger, postman or any other methods.

The api is exposed on port  ```888```