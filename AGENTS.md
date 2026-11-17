# Working on Servion

Servion is Inticate Softworks' Docker container management panel, primarily for game servers. Its end users should be able to create and manage servers with little technical knowledge. Keep UI text and workflows clear and approachable.

This guide applies across the repository. Use the current source code and configuration as the authority when behavior changes, and update this guide when changing commands or project structure.

## Stack and repository map

This is a Bun workspace with a React frontend and a separate ASP.NET Core backend. Database access belongs to the backend through Entity Framework Core and PostgreSQL.

| Path                                   | Purpose                                                                                                                                                                                     |
| -------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `apps/web/`                            | React 19, TypeScript, TanStack Start/Router, React Query, Vite, and Nitro SSR application.                                                                                                  |
| `apps/web/src/routes/`                 | File-based routes. `__root.tsx` owns the document shell, metadata, global stylesheet, and shared boundaries; `index.tsx` is the protected server dashboard; `auth/login.tsx` handles login. |
| `apps/web/src/components/ui/`          | Product components grouped under `server/` and `navigation/`; reusable shadcn/Radix primitives live under `primitives/`.                                                                    |
| `apps/web/src/lib/`                    | Auth requests, server functions, query-client construction, and class-name utilities.                                                                                                       |
| `apps/web/src/hooks/`                  | React hooks, including `use-auth.ts`.                                                                                                                                                       |
| `apps/web/src/middlewares/`            | TanStack Start server middleware, including cookie forwarding and authentication checks.                                                                                                    |
| `apps/web/src/utility/`                | Frontend helpers such as SEO utilities.                                                                                                                                                     |
| `apps/web/src/styles/app.css`          | Tailwind CSS v4 imports, shared theme tokens, light/dark palettes, and base styles.                                                                                                         |
| `apps/web/src/generated/api/`          | Orval-generated API clients, query hooks, and models.                                                                                                                                       |
| `apps/web/public/`                     | Static assets, icons, and web manifest.                                                                                                                                                     |
| `apps/api/`                            | ASP.NET Core API targeting .NET 10; `Program.cs` configures services, routes, Docker, Identity, OpenAPI, and background jobs.                                                               |
| `apps/api/Controllers/`                | HTTP endpoints and request/response orchestration.                                                                                                                                          |
| `apps/api/Services/`                   | Domain operations for servers, users, environments, health, locations, backends, and email.                                                                                                 |
| `apps/api/Models/`                     | Persistence/domain models, including Identity users and container/server relationships.                                                                                                     |
| `apps/api/DTO/`, `Validators/`         | Request DTOs and FluentValidation rules.                                                                                                                                                    |
| `apps/api/Contexts/DatabaseContext.cs` | EF Core Identity context, entity sets, and modification timestamp handling.                                                                                                                 |
| `apps/api/Migrations/`                 | EF Core migrations and model snapshot.                                                                                                                                                      |
| `apps/api/Exceptions/`, `Utilities/`   | Domain exceptions, ProblemDetails handling, and environment-variable validation.                                                                                                            |
| `apps/api/Jobs/`                       | Quartz cleanup and health jobs, registered in `Program.cs`.                                                                                                                                 |
| `packages/config/`                     | Shared app settings and validated server/client environment variables.                                                                                                                      |
| `packages/utility/`                    | LogTape logging, shared TypeScript types, and application errors; errors are exported through `@servion/utility/errors`.                                                                    |
| `packages/validation/`                 | Shared Zod schemas; currently contains a minimal user-creation schema.                                                                                                                      |
| `justfile`                             | Backend run, build, formatting, and EF migration recipes.                                                                                                                                   |
| `.github/workflows/`                   | Bun lint/typecheck and .NET formatting/build checks.                                                                                                                                        |

Workspace packages export TypeScript source directly; they do not have separate build scripts. Use their `@servion/*` exports instead of reaching into sibling package source paths. The web app's `@/*` alias resolves to `apps/web/src/*`.

## Setup and local development

Run commands from the repository root unless a different directory is explicitly stated.

1. Install Bun, Node.js (used by the production web server), .NET SDK 10, and Just. EF migration commands also require the `dotnet-ef` tool compatible with EF Core 10. There is no checked-in tool manifest or exact Bun/Node version pin.
2. Run `bun install`. Use `bun install --frozen-lockfile` to reproduce the checked-in dependency resolution, as CI does. Keep `bun.lock` synchronized with intentional dependency changes; do not introduce another package manager's lockfile.
3. Copy `.env.example` to `.env` if it does not already exist and replace placeholders with working local values. Do not overwrite an existing developer environment.
4. Provide PostgreSQL and a reachable Docker daemon, plus SMTP settings. The example file is a template, not a working database/service setup.
5. Run `just api` and `bun run web:dev` in separate terminals. The web development port is `3000`, set in `packages/config/src/index.ts`. The API binds localhost on `SERVER_PORT` (`8080` in the example), overriding the apparent ports in `launchSettings.json`.

The root web and Orval wrappers explicitly load root `.env`. Vite also sets its environment directory to the repository root. The API uses `Env.Load("../../.env")`; its relative path relies on the API working directory. If running outside the normal project launch flow, check the working directory and environment loading.

### Environment configuration

`packages/config/src/index.ts` validates environment variables using Zod and `@t3-oss/env-core`; `apps/api/Utilities/EnvironmentVariableUtility.cs` validates the backend's required settings.

| Variable                              | Meaning                                                                                                        |
| ------------------------------------- | -------------------------------------------------------------------------------------------------------------- |
| `DATABASE_URL`                        | PostgreSQL connection setting passed to the API's Npgsql provider; use a connection string accepted by Npgsql. |
| `DOCKER_PATH`                         | Docker daemon endpoint URI. Verify that the example socket path matches the local daemon.                      |
| `SERVER_PORT`                         | API HTTP port; also used by the web proxy, SSR auth requests, and Orval.                                       |
| `PUBLIC_SERVER_URL`                   | Browser-facing API origin used by frontend auth requests.                                                      |
| `SMTP_HOST`, `SMTP_PORT`              | SMTP connection settings.                                                                                      |
| `SMTP_USER`, `SMTP_PASSWORD`          | SMTP credentials.                                                                                              |
| `SMTP_FROM_ADDRESS`, `SMTP_FROM_NAME` | Sender identity.                                                                                               |
| `NODE_ENV`                            | Shared TypeScript server environment: `development` by default, or `production`.                               |
| `FORCE_COLOR`                         | Optional numeric setting, defaulting to `1`.                                                                   |

Only variables prefixed `PUBLIC_` are exposed through Vite's configured client environment. Keep secrets out of public variables and client modules. Importing shared configuration can validate more settings than a command directly needs, so use the complete local environment when diagnosing startup or generation failures. Never commit `.env` or real credentials.

## JavaScript/TypeScript commands

Bun is the package manager and script runner, despite these being standard `package.json` scripts. These tables cover every current root and web workspace script.

### Root scripts

| Command                  | What it does                                                                                                                                                            |
| ------------------------ | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `bun run web:dev`        | Loads root `.env` and runs `@servion/web`'s `dev` script (`vite dev`).                                                                                                  |
| `bun run web:start`      | Loads root `.env` and runs the built Nitro server using Node.js. Requires a web build first.                                                                            |
| `bun run orval:generate` | Loads root `.env` and generates the web API client from the running API's OpenAPI document.                                                                             |
| `bun run orval:watch`    | Runs the same Orval generation in watch mode. Requires the OpenAPI source to remain accessible.                                                                         |
| `bun run prepare`        | Runs Husky to install Git hooks; also used as the package lifecycle prepare script.                                                                                     |
| `bun run format`         | Runs `oxfmt .`, writing formatting changes across its configured scope. It is not a read-only check.                                                                    |
| `bun run lint`           | Runs `oxlint .` with type-aware linting/type checking configured. It does not pass an automatic-fix flag.                                                               |
| `bun run typecheck`      | Runs workspace `typecheck` scripts where present. Currently only the web workspace declares one; this does not explicitly typecheck every shared package independently. |
| `bun run stats`          | Uses `bunx cloc` to count `.ts`/`.tsx` source lines, excluding the directories listed in the script. This is informational, not validation.                             |

There is no root build, preview, or test script. Use the explicit workspace commands below for web builds and previews.

### Web workspace scripts

The following commands select the web workspace from the root. Unlike the root wrappers, these commands do not explicitly pass `--env-file=.env`; ensure required environment variables are available. Vite itself reads the configured root environment directory.

| Command                                     | Underlying script and behavior                                                                                                                         |
| ------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `bun run --filter '@servion/web' dev`       | `vite dev`: starts the web development server. Prefer `bun run web:dev` for the root environment wrapper.                                              |
| `bun run --filter '@servion/web' build`     | `vite build && tsc --noEmit`: builds the web app, then checks TypeScript. Production server output is expected at `apps/web/.output/server/index.mjs`. |
| `bun run --filter '@servion/web' preview`   | `vite preview`: previews a built application locally.                                                                                                  |
| `bun run --filter '@servion/web' start`     | `node .output/server/index.mjs`: starts the built production server. Prefer `bun run web:start` for root environment loading.                          |
| `bun run --filter '@servion/web' typecheck` | `tsc --noEmit`: checks the web TypeScript project without producing output.                                                                            |
| `bun run --filter '@servion/web' orval`     | `orval`: generates clients using `apps/web/orval.config.ts`. Prefer the root Orval wrappers for environment loading.                                   |

For explicit environment loading with a workspace build, use `bun --env-file=.env run --filter '@servion/web' build`.

## Justfile and .NET commands

| Command                     | What it does                                                                                                                                                |
| --------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `just` or `just default`    | Lists available recipes using `just --list`.                                                                                                                |
| `just api`                  | `dotnet run --project apps/api`: builds and starts the API.                                                                                                 |
| `just build-api`            | `dotnet build apps/api`: builds the backend in the default configuration.                                                                                   |
| `just format-api`           | `dotnet format apps/api --no-restore`: writes backend formatting changes; restore dependencies first.                                                       |
| `just format-check-api`     | Intended to check formatting, but currently passes the misspelled `--verifiy-no-changes`. Use the corrected direct command below until the recipe is fixed. |
| `just migration-api <name>` | `dotnet ef migrations add <name> --project apps/api`: generates an EF migration for model changes.                                                          |
| `just migrate-api`          | `dotnet ef database update --project apps/api`: applies pending migrations to the configured database.                                                      |
| `just migrations-api`       | `dotnet ef migrations list --project apps/api`: lists EF migrations.                                                                                        |
| `just migration-remove-api` | `dotnet ef migrations remove --project apps/api`: removes the latest migration, subject to EF's database-state checks.                                      |

Backend CI uses these direct commands:

```sh
dotnet restore apps/api
dotnet format apps/api --verify-no-changes --no-restore
dotnet build apps/api --configuration Release --no-restore
```

Database schema changes belong in C# models/context and EF migrations. Review both the generated migration and model snapshot. Migration commands may need valid startup configuration and database connectivity. Applying migrations changes the configured database; verify the target before running them. Do not rewrite already-applied migration history to implement a new change.

## API contracts, routing, and authentication

- Backend endpoints live under `/api/`; inspect the relevant controller and service before changing a request or response. Follow existing DTO, validator, domain exception, and ProblemDetails patterns. Register new services and validators in `Program.cs` when needed.
- In development, the API exposes `/openapi/v1.json` and Scalar documentation at `/docs`. OpenAPI is only mapped when the ASP.NET environment is Development.
- Orval reads `http://localhost:${SERVER_PORT}/openapi/v1.json`. It generates fetch-based React Query clients split by tags, with models under `src/generated/api/models`, a 10-second query freshness default, abort signals, and prefetch support. Its output has `clean: true`; do not put handwritten code in the generated directory.
- After changing an API contract, start the API in Development, run `bun run orval:generate`, and review the generated diff alongside affected callers. Do not manually patch generated API models or hooks.
- TanStack Router generates `apps/web/src/routeTree.gen.ts`. Add or edit files in `src/routes/` and let the Vite/TanStack tooling update the route tree rather than editing it manually.
- Nitro config proxies `/api/**` to the local API. Browser auth code also uses `PUBLIC_SERVER_URL` directly, while SSR auth forwards the incoming cookie to localhost on `SERVER_PORT`. Preserve the intended cookie flow when changing origins or requests.
- Protected route loading uses `requireUser` and `authenticatedMiddleware`; unauthorized requests redirect to `/auth/login`. Backend Identity/authorization remains the authority for protected API operations.
- `router.tsx` creates a separate React Query client for each router/server request and integrates SSR hydration. Preserve this isolation; do not introduce a global user-specific query cache.
- The root dashboard currently renders local example server data. Do not assume those rows or all UI actions are connected to real API operations.

## Code style

### TypeScript and React

Treat `oxfmt.config.ts`, `oxlint.config.ts`, and the web-specific extensions as the executable style rules. Match nearby source patterns while allowing the formatter to settle layout.

- Use two-space indentation, double quotes, semicolons, LF line endings, and no trailing commas. The formatter's print width is 120, with bracket spacing, closing JSX brackets on a separate line, and omitted parentheses around a single arrow parameter where possible.
- Use strict TypeScript and ESM imports/exports. Prefer `import type` for type-only imports. Respect the different root and web `tsconfig.json` settings; the web config is independent of the root config.
- Use PascalCase for components and interfaces, camelCase for functions and variables, and the existing kebab-case frontend filenames. Components generally use typed arrow functions and default exports; routes export the named `Route` required by TanStack Router.
- Document exported helpers, components, interfaces, and non-obvious behavior with concise JSDoc. Existing files often group imports under `// Resources`, `// Components`, or `// Plugins`, and implementation sections under short comments. Follow local organization without adding redundant commentary.
- Provide explicit parameter and return types where the existing lint rules require them. Use existing shared types such as `Possible<T>`, `Children`, and `ClassName` where appropriate.
- Prefer `undefined` for absent values in handwritten TypeScript; the base linter enables `unicorn/no-null`. Existing boundary checks against JSON `null` do not justify unnecessary nullable application state.
- Handle promises intentionally. The base rules require async promise functions and return-await handling; the web configuration rejects floating promises. Use `await`, error handling, or intentional `void` where suitable.
- JSX string attributes and text follow the existing expression style, e.g. `className={"..."}` and `{"Servers"}`; the React lint configuration requires curly braces.
- Reuse primitives under `components/ui/primitives/` and product components before adding new abstractions. Use Tailwind v4 utilities, existing theme tokens, `cn` from `@/lib/utils`, and existing variant patterns. Icons use Lucide; interactive primitives use Radix/shadcn conventions.
- Preserve keyboard access, labels, focus states, responsive layouts, and pending/error/empty states when changing UI. Keep browser-only APIs out of server execution paths.
- Generated API files and the route tree have web-specific lint/format exclusions. Do not fix generated style by hand or weaken repository-wide rules to accommodate a local change.

### C#

- Follow `dotnet format` and existing ASP.NET conventions: four-space indentation, file-scoped `API.*` namespaces, braces on separate lines, PascalCase types/methods/properties, and camelCase parameters/locals. Existing private instance fields use an underscore prefix.
- Nullable reference types, implicit usings, latest analysis, and style enforcement in builds are enabled in `API.csproj`. Keep nullability accurate.
- Match existing XML documentation on public types and methods. Use primary constructors for dependency injection where neighboring code does, and keep HTTP orchestration in controllers with domain work in services.
- Follow existing async EF/Docker calls, DTO records/types, and FluentValidation classes. Do not assume a validator is automatically executed merely because it exists; check its registration and call path.
- Preserve the Identity model and authorization checks when changing user-facing operations. Use established domain exceptions and the global ProblemDetails handler for expected API failures.
- Use the context's existing timestamp handling and review entity relationships when changing persistence. Generated migrations and snapshots should come from EF tooling.

## Efficient change and validation workflow

1. Read the relevant route/component or controller/service and its callers. Search with `rg` and exclude generated output when it obscures handwritten code.
2. Make the smallest coherent change in the owning app/package. Avoid unrelated reformatting, IDE settings, generated build output, or dependency churn.
3. Update API clients or route generation through their tools when applicable. Review the resulting diff rather than assuming generation is correct.
4. Run checks appropriate to the change:
   - TypeScript/frontend: `bun run lint` and `bun run typecheck`; run the web build for bundling, SSR, or configuration changes.
   - C#: restore as needed, then the corrected format check and backend build shown above.
   - UI/auth/API behavior: exercise the affected flow with the necessary local services; check loading, error, and unauthorized outcomes as relevant.
   - Documentation-only: check paths, commands, and the diff; a full application build is generally unnecessary.
5. Report what changed, what was checked, and any checks blocked by missing services or configuration. Do not describe an unrun check as passing.

There is currently no checked-in automated test suite or test script. Lint, typecheck, format checks, and builds are the existing automated verification. Add meaningful tests when the task warrants them without inventing a pre-existing test runner.

Root formatting writes files and excludes the API; use `dotnet format` for C#. For a focused TypeScript formatting pass, invoke `bunx oxfmt <changed-paths>` rather than reformatting the entire repository unnecessarily.

## Commits and contribution context

Husky's pre-commit hook runs `bunx lint-staged`; the commit-msg hook runs `bunx commitlint --edit $1`. Commit messages follow `@commitlint/config-conventional`, for example `fix(web): handle expired sessions`.

The current lint-staged configuration maps `**/*.{ts,tsx}` to `bun run lint`, whose script already targets `.`, so a staged change can trigger repository-wide linting. Its formatting pattern is `src/**/*.{ts,tsx}`, which does not cover the usual root-relative `apps/web/src/` or `packages/*/src/` paths. Run formatting explicitly for touched files instead of assuming the hook covered them.

Read `CONTRIBUTING.md` and `CODE_OF_CONDUCT.md` for contribution expectations. `SECURITY.md` directs ordinary security reports to GitHub issues and sensitive reports to `contact@inticate.com`; do not put sensitive report details in public issues.
