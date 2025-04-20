# DotGPT

## Introduction

DotGPT is a custom application built with .NET. It provides a dedicated interface and backend for interacting with Large Language Models (LLMs).

## Description

This project consists of:

*   **DotGPT.Server:** An ASP.NET Core backend responsible for handling API requests and potentially communicating with LLM services.
*   **DotGPT.Client:** A Blazor WebAssembly frontend providing the user interface for interacting with the backend and displaying LLM responses.
*   **Supporting Projects (Core, Application, Infrastructure):** Likely containing shared logic, business rules, and infrastructure concerns.

The application is containerized using Docker and intended to be run via the `docker-compose.yml` file within this directory, or as part of the main `compose.yml` in the root of the `local-llm` repository.

## Idea

The purpose of DotGPT within the `local-llm` project is to offer a .NET-centric alternative or specialized tool for LLM interaction. It can serve as:

*   A platform for experimenting with .NET-specific LLM libraries and integrations.
*   A testbed for developing custom UI/UX features for chat interfaces.
*   An Blazor application chat with local LLM backends.
