FROM zensical/zensical:latest AS build

WORKDIR /app

COPY zensical.toml .
COPY docs/ ./docs/

RUN zensical build

FROM nginx:alpine as proxy

COPY --from=build /app/site /ur/share/nginx/html

EXPOSE 80
