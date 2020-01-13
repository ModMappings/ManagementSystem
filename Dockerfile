FROM gradle:jdk11 as builder

COPY --chown=gradle:gradle . /home/gradle/src
WORKDIR /home/gradle/src
RUN ./gradlew build

FROM alpine as extractor
COPY --from=builder /home/gradle/src/source/api/build/distributions/api-boot.tar /app/
WORKDIR /app
RUN tar -xvf api-boot.tar

FROM openjdk:11-jdk-slim
COPY --from=extractor /app/ /app/
WORKDIR /app/api-boot/
CMD bin/api