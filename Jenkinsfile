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
                    args '-v mmmsgc:/home/gradle/.gradle/'
                }
            }
            environment {
                MMMS_MAVEN = credentials('nexus-mmms')
            }
            steps {
                sh './gradlew ${GRADLE_ARGS} clean test build'
                script {
                    env.MYVERSION = sh(returnStdout: true, script: './gradlew ${GRADLE_ARGS} properties -q | grep "version:" | cut -d\' \' -f 2').trim()
                }
                stash includes: 'source/api/build/distributions/api-boot.tar', name: 'app'
                sh './gradlew ${GRADLE_ARGS} -PmmmsMavenUser=${MMMS_MAVEN_USR} -PmmmsMavenPassword=${MMMS_MAVEN_PSW} publish'
            }
            post {
                success {
                    archiveArtifacts artifacts: 'source/api/build/distributions/api-boot.tar', fingerprint: true
                    archiveArtifacts artifacts: 'source/*/build/libs/*.jar', fingerprint: true
                }
            }

        }
        post {
            success {
                unstash 'app'
                script {
                    def img = docker.image('tmaier/docker-compose:1.12')
                    img.inside('-v /var/run/docker.sock:/var/run/docker.sock')
                    {
                        sh '/usr/bin/docker-compose up -d --force-recreate --remove-orphans'
                    }
                }
            }
        }
    }

}