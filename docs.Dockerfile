FROM zensical/zensical:latest AS build

WORKDIR /app

COPY zensical.toml .
COPY docs/ ./docs/

RUN zensical build

FROM nginx:alpine AS proxy

RUN rm -rf /usr/share/nginx/html/* && mkdir -p /usr/share/nginx/html/servion

COPY --from=build /app/site/ /usr/share/nginx/html/servion/

EXPOSE 80
