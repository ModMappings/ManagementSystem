FROM openjdk:11-jdk-slim
ADD api-boot.tar /app/
WORKDIR /app/api-boot/
CMD bin/api