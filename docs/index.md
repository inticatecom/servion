---
icon: lucide/house
---

# Servion

Servion is a game [Docker](https://docker.com) management interface. For our users who don't know what Docker is, it is a containerization platform which allows for programs to run in an isolated environment away from the host operating system but within the same machine.

!!! warning
    Servion is in it's earliest stages of development. Expect issues. We do not recommend usage in production yet. Contributions are welcome.

The below information will help you understand how Servion functions, it's terminology and other information that may help you further understand it's role.

## Terminology
Terms or keywords that will be used throughout the documentation and the codebase related specifically to the panel.

### `Backend`
A .NET instance separated from the primary codebase deployed to their own machines. A back-end is where servers will actually be created. This allows for expansion to multiple machines when deploying servers as you probably won't want all of your servers deployed on the root machine as you scale.
