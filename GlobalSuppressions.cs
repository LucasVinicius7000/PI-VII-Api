﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE0058:O valor da expressão nunca é usado", Justification = "<Pendente>", Scope = "member", Target = "~M:LocalStore.Program.Main(System.String[])")]
[assembly: SuppressMessage("Style", "IDE0058:O valor da expressão nunca é usado", Justification = "<Pendente>", Scope = "member", Target = "~M:LocalStore.Infra.Data.Context.LocalStoreDbContext.OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder)")]
[assembly: SuppressMessage("Usage", "CA2201:Não gerar tipos de exceção reservados", Justification = "<Pendente>", Scope = "member", Target = "~M:LocalStore.Infra.BlobStorage.Implementations.BlobStorageService.UploadFile(System.String,System.String,System.IO.Stream)~System.Threading.Tasks.Task{System.String}")]
[assembly: SuppressMessage("Style", "IDE0008:Usar o tipo explícito", Justification = "<Pendente>", Scope = "member", Target = "~M:LocalStore.Infra.BlobStorage.Implementations.BlobStorageService.UploadFile(System.String,System.String,System.IO.Stream)~System.Threading.Tasks.Task{System.String}")]
[assembly: SuppressMessage("Style", "IDE0058:O valor da expressão nunca é usado", Justification = "<Pendente>", Scope = "member", Target = "~M:LocalStore.Controllers.WeatherForecastController.Get~System.Threading.Tasks.Task{System.Collections.Generic.IEnumerable{LocalStore.WeatherForecast}}")]
[assembly: SuppressMessage("Usage", "CA2201:Não gerar tipos de exceção reservados", Justification = "<Pendente>", Scope = "member", Target = "~M:LocalStore.Controllers.WeatherForecastController.Get~System.Threading.Tasks.Task{System.Collections.Generic.IEnumerable{LocalStore.WeatherForecast}}")]
[assembly: SuppressMessage("Usage", "CA2201:Não gerar tipos de exceção reservados", Justification = "<Pendente>", Scope = "member", Target = "~M:LocalStore.Controllers.WeatherForecastController.Get(System.String,System.String)~System.Threading.Tasks.Task{System.Collections.Generic.IEnumerable{LocalStore.WeatherForecast}}")]
