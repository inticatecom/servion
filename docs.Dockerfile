FROM zensical/zensical:latest AS build

WORKDIR /app

COPY zensical.toml .
COPY docs/ ./docs/

RUN zensical build

FROM nginx:alpine as proxy

RUN rm -rf /usr/share/nginx/html/*

COPY --from=build /app/site/ /usr/share/nginx/html/

EXPOSE 80
