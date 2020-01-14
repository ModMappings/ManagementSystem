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
                sh './gradlew build'
                stash includes: 'source/api/build/distributions/api-boot.tar', name: 'app'
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
                unstash 'app'
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