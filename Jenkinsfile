pipeline {
    agent none
    environment {
        GRADLE_ARGS = '--no-daemon'
    }
    stages {
        stage('gradle') {
            agent {
                docker {
                    image 'gradle:jdk11'
                    args '-v modmappingapigradlecache:/home/gradle/.gradle/'
                }
            }
            steps {
                sh './gradlew ${GRADLE_ARGS} --stop'
                sh './gradlew ${GRADLE_ARGS} build'
                script {
                    env.MYVERSION = sh(returnStdout: true, script: './gradlew ${GRADLE_ARGS} properties -q | grep "version:" | cut -d\' \' -f 2').trim()
                }
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
                    site=docker.build("modmappingapi:${env.BUILD_ID}", ".")
                    site.tag("v${env.MYVERSION}")
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