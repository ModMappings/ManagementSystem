FROM alpine as extractor
COPY source/api/build/distributions/api-boot.tar /app/
WORKDIR /app
RUN tar -xvf api-boot.tar

FROM openjdk:11-jdk-slim
COPY --from=extractor /app/ /app/
WORKDIR /app/api-boot/
CMD bin/api