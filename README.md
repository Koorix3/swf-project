# Test Scenario Framework
This framework can be used to create entities and populate their fields with randomly generated data. It was created as a project for the course “Software Frameworks” at the University of Applied Sciences Technikum Wien (www.technikum-wien.at).

![Build Status](https://koorix3.visualstudio.com/_apis/public/build/definitions/11d0b104-f77d-4378-abe8-470aa0cb0125/4/badge)

## Features
- Creation of test entities with minimal code overhead.
- Entity classes can be created simply by using attributes.
- Entity fields can be populated with randomly generated data.
- Data generation can be controlled (range, format, …) with attributes.
- Generated data sets can be persisted in the file system as JSON files.
- JSON scenario files can be edited to create specific test scenarios.

## Installation
The framework can be installed via the NuGet Package Manager.

```
PM> Install-Package FHTW.SFW.TestScenarioFramework -Version 0.1.0
```

## Getting Started
Entity classes can be prepared for generation by using the *TestScenarioEntity* and *TestScenarioMember* attributes, as shown in the following listing.
```csharp
[TestScenarioEntity]
public class Movie
{
    public int MovieId { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }

    [TestScenarioMember(Min = "1970-01-01", Max = "2018-04-01")]
    public DateTime ReleaseDate { get; set; }

    [TestScenarioMember(Min = 50, Max = 60)]
    public decimal Price { get; set; }

    [TestScenarioMember(Multiplicity = 5)]
    public virtual List<Actor> Actors { get; set; }

    public virtual Actor Lead { get; set; }
}
```
The virual propery *Actor* in this example also uses a type that is marked as a *TestScenarioEntity* and thus gets initiated by the framework.

```csharp
[TestScenarioEntity]
public class Actor
{
    public int ActorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int MovieId { get; set; }

    // Avoid cyclic references for mock data generation!
    [TestScenarioMember(Exclude = true)]
    public Movie Movie { get; set; }
}
```
Mind the field *Movie*, which uses the *Movie* datatype and is excluded from the data generator to avoid infinite generation cycles.

After marking the entity types with those attributes, they can be initialized dynamically by the framework.

```csharp
// Initialize exporter (For testing. Exporters should be constructed by DI-frameworks.)
JsonExporter exporter = new JsonExporter();

// Create a new TestScenario instance
var scenario = new TestScenario("hello_world", exporter);

// Create a random entity on-the-fly
var someMovie = scenario.GetEntity<Entites.Movie>();
```

Using *JsonExporter*, the generated entity objects and data can be persisted by calling the *Save* method.

```csharp
scenario.Save();
```

This creates the JSON-file in the following listing, which can be modified an re-read by the framework.

```json
[
  {
    "MovieId": 39,
    "Title": "upkcbgxffy",
    "Genre": "qicdfhunqa",
    "ReleaseDate": "2017-09-25T21:28:11.690478",
    "Price": 58.69,
    "Actors": [
      {
        "ActorId": 4,
        "FirstName": "geftgprczt",
        "LastName": "ljlxxvdbzc",
        "MovieId": 26,
        "Movie": null
      },
      {
        "ActorId": 66,
        "FirstName": "fcglmnyzjk",
        "LastName": "ynexvnrpnw",
        "MovieId": 67,
        "Movie": null
      },
      {
        "ActorId": 57,
        "FirstName": "qttjwrnzau",
        "LastName": "uozdnvdeez",
        "MovieId": 36,
        "Movie": null
      },
      {
        "ActorId": 4,
        "FirstName": "lxszaqjkok",
        "LastName": "zqmmrcqgmj",
        "MovieId": 39,
        "Movie": null
      },
      {
        "ActorId": 15,
        "FirstName": "tpyszwljrr",
        "LastName": "fpivqxrxqq",
        "MovieId": 20,
        "Movie": null
      }
    ],
    "Lead": {
      "ActorId": 37,
      "FirstName": "pjqbdbnxnz",
      "LastName": "qhedbhqnyf",
      "MovieId": 72,
      "Movie": null
    }
  }
]
```

## Solution Structure

The solutions contains several projects:

- *TestScenarioFramework*: The test scenario framework
- *TestScenarioFramework.UnitTests*: Unit tests for the framwork
- *MvcMovie*: Sample .Net CORE MVC application
- *MvcMovie.UnitTests*: Unit tests for the sample application
- *LocalDebug*: Console application project for simple framework testing
