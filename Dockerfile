FROM openjdk:11-jdk-slim
WORKDIR /app/api-boot/
ADD source/api/build/distributions/api-boot.tar /app/
CMD bin/api