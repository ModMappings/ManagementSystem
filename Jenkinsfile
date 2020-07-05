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
                sh './gradlew ${GRADLE_ARGS} --continue clean test build'
            }
            post {
                success {
                    archiveArtifacts artifacts: 'source/api/build/distributions/api-boot.tar', fingerprint: true
                    archiveArtifacts artifacts: 'source/*/build/libs/*.jar', fingerprint: true
                    sh './gradlew ${GRADLE_ARGS} -PmmmsMavenUser=${MMMS_MAVEN_USR} -PmmmsMavenPassword=${MMMS_MAVEN_PSW} publish'
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
}