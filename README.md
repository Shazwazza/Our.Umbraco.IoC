# Umbraco.IoC

A project containing various dependency injection containers for Umbraco

## Installation

All installation is done via Nuget

The base/core package is

	Install-Package Our.Umbraco.IoC

Then it depends on what container you want to use and what containers this package currently supports:

### Autofac support

	Install-Package Our.Umbraco.IoC.Autofac

### LighInject support

	Install-Package Our.Umbraco.IoC.LightInject

## Contributing

Contribution is fairly easy for this project. You should be able to clone the repository and build the solution which should install all nuget dependencies and then you can run the website which will execute the Umbraco installer.

It would be great to have support for a number of IoC Containers so if your favorite isn't listed than let's add it :)

### Project structure

In order to minimize code duplication, all Umbraco service registrations are declared in the Our.Umbraco.IoC project in the `UmbracoServices` class. 
The `GetAllRegistrations` method on this class is used to return all required container registrations for Umbraco. The return type is `IEnumerable<IContainerRegistration>`.

Then you need to get these registrations into your container type and this will very much depend on how your container works. There's 2 examples of how this is done currently:

* Autofac - `Our.Umbraco.IoC.Autofac.AutofacUmbracoRegister` which uses Autofac's `IRegistrationSource` to register components
* LightInject - `Our.Umbraco.IoC.Autofac.LightInjectUmbracoRegister` which uses LightInject's `ICompositionRoot` to register components

Each project should be structured consistently with the existing ones, it's only a few classes to get a new container going.