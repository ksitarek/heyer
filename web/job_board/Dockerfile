FROM node:23-alpine AS build
WORKDIR /app

COPY web/package.json web/package-lock.json ./
RUN npm install

COPY web .

ARG CONFIGURATION=production
RUN npm run build -- --configuration ${CONFIGURATION}

FROM node:23-alpine
COPY --from=build /app/dist/heyer ./

CMD ["node", "server/server.mjs"]
EXPOSE 4000
