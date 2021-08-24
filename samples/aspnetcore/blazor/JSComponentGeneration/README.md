# JavaScript Component Generation

This sample demonstates how Blazor components can be automatically wrapped as components in JavaScript-based SPA frameworks like Angular and React.

## Running the Angular sample

Clone this repo. Then, in a command prompt, execute:

 * `cd BlazorAppGeneratingJSComponents`
 * `dotnet watch`

Leave that running, and open a second command prompt, and execute:

 * `cd angular-app-with-blazor`
 * `npm install`
 * `npm start`

Now when you browse to http://localhost:4200/, you'll see an Angular application that dynamically renders Blazor WebAssembly components, passing parameters to them.

## Running the React sample

Clone this repo. Then, in a command prompt, execute:

 * `cd BlazorAppGeneratingJSComponents`
 * `dotnet watch`

Leave that running, and open a second command prompt, and execute:

 * `cd react-app-with-blazor`
 * `yarn install`
 * `yarn start`

Now when you browse to http://localhost:3000/, you'll see a React application that dynamically renders Blazor WebAssembly components, passing parameters to them.

## Converting a Blazor Component to an Angular or React component
To indicate to the MSBuild tasks that an Angular or React wrapper should be generated for a Blazor component, you can add these attributes to the component:

_MyComponent.razor:_
```razor
@attribute [GenerateAngular] // Generate an Angular component
@attribute [GenerateReact]   // Generate a React component

// ...
```

In order to use these components from an Angular or React app, you need to register them in `Program.Main`:

_Program.cs:_
```csharp
var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.RegisterForAngular<MyComponent>(); // Register for Angular
builder.RootComponents.RegisterForReact<MyComponent>();   // Register for React

// ...
```

A generated JS component will accept parameters correlating with the parameters accepted by the Blazor component. Following are the supported parameter types:
* Built-in types (`bool`, `int`, `string`, etc.)
* Complex types (JSON-serializable)
* `EventCallback` and `EventCallback<T>` types

## Configuring the JS component generation build task
The tasks that generate the React and Angular components can be configured in `JSComponentGeneration.Build/build/netstandard2.0/JSComponentGeneration.Build.targets` (via `GenerateAngularComponents` and `GenerateReactComponents`). The main property of interest is `JavaScriptComponentOutputDirectory`, which lets you specify the directory where the JS components should be generated.

If you would like to change _how_ the components are generated, you can modify the tasks themselves, located in `JSComponentGeneration.Build/Angular/GenerateAngularComponents` and `JSComponentGeneration.Build/React/GenerateReactComponents` for Angular and React, respectively. You may want to do this if, for example, you have a React project built with TypeScript (this sample generates React components in JavaScript).
