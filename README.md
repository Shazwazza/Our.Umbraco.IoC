[![Build status](https://ci.appveyor.com/api/projects/status/t04tla7g6k89oq6k?svg=true)](https://ci.appveyor.com/project/Shandem/our-umbraco-ioc)

---
_❤️ If you use and like Our.Umbraco.IoC please consider [becoming a GitHub Sponsor](https://github.com/sponsors/Shazwazza/) ❤️_

# Our.Umbraco.IoC

The fastest and easiest way to add Dependency Injection support to your Umbraco 7.x project with support for various container types.

## Installation

All installation is done via Nuget

The base/core package is

	Install-Package Our.Umbraco.IoC

Then it depends on what container you want to use and what containers this package currently supports:

### Autofac support

	Install-Package Our.Umbraco.IoC.Autofac

If you want to disable this container's support you can do so by setting this appSetting. If this appSetting doesn't exist, then the default value is 'true'

```xml
<add key="Our.Umbraco.IoC.Autofac.Enabled" value="false" />
```

### Unity support

	Install-Package Our.Umbraco.IoC.Unity

**Important** The Unity nuget package will install some files in the /App_Start folder: `UnityConfig.cs`, `UnityMvcActivator.cs`, `UnityWebApiActivator.cs` and it is recommended to delete these files. These boilerplate files are used to configure a Unity container and manage startup/shutdown of the various Unity components, however this package `Our.Umbraco.IoC.Unity` does all of this boilerplate work for you and uses it's own Unity container. See the [Using the Container](https://github.com/Shazwazza/Our.Umbraco.IoC/wiki/Using-the-Container) docs for info on how to customize this container.

If you want to disable this container's support you can do so by setting this appSetting. If this appSetting doesn't exist, then the default value is 'true'

```xml
<add key="Our.Umbraco.IoC.Unity.Enabled" value="false" />
```

### LighInject support

	Install-Package Our.Umbraco.IoC.LightInject

If you want to disable this container's support you can do so by setting this appSetting. If this appSetting doesn't exist, then the default value is 'true'

```xml
<add key="Our.Umbraco.IoC.LightInject.Enabled" value="false" />
```

#### LightInject support on Umbraco Cloud (with v7.x)

This version is identical in functionality to the above `LightInject` implementation, however because Umbraco Cloud runs with Umbraco Deploy which has a dependency on `LightInject 4.x` this version is created to accommodate that.

	Install-Package Our.Umbraco.IoC.LightInject4

If you want to disable this container's support you can do so by setting this appSetting. If this appSetting doesn't exist, then the default value is 'true'

```xml
<add key="Our.Umbraco.IoC.LightInject.Enabled" value="false" />
```


## Using the container

Click [here for docs on using the container](https://github.com/Shazwazza/Our.Umbraco.IoC/wiki/Using-the-Container).

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

__Important!__ Most of these registrations are Externally Owned, meaning Umbraco owns the lifetime of the objects. When you are building the code for your container, you need
to make sure that if an `IContainerRegistration`'s lifetime is `ExternallyOwned` that you deal with that accordingly. Most containers will have a way that you can define this
so that the container doesn't Dispose of the object itself.
