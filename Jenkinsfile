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
                sh './gradlew build'
            }
            post {
                success {
                    archiveArtifacts artifacts: 'source/api/build/distributions/api-boot.tar', fingerprint: true
                }
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
                copyArtifacts filter: 'api-boot.tar', fingerprintArtifacts: true, projectName: '${JOB_NAME}', selector: specific('${BUILD_NUMBER}')
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