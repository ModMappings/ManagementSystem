pipeline {
    agent none
    stages {
        stage('gradle') {
            agent {
                docker {
                    image 'gradle:jdk11'
                }
            }
            steps {
                checkout scm
                sh '/home/gradle/src/gradlew build'
            }
        }
        stage('docker') {
            agent {
                docker {
                    image 'docker:latest'
                    args '-v /var/run/docker.sock:/var/run/docker.sock'
                }
            }
            steps {
                script {
                    site=docker.build("modmappingapi:${env.BUILD_ID}")
                    site.tag("latest")
                }
            }
        }
//         post {
//             success {
//                 script {
//                     def img = docker.image('tmaier/docker-compose:1.12')
//                     img.inside('-v /var/run/docker.sock:/var/run/docker.sock')
//                     {
//                         sh '/usr/bin/docker-compose up -d --force-recreate --remove-orphans'
//                     }
//                 }
//             }
//         }
    }

}