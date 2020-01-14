FROM openjdk:11-jdk-slim
ADD source/api/build/distributions/api-boot.tar /app/
WORKDIR /app/api-boot/
CMD bin/api