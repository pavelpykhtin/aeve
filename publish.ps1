$aeveImage = './out/images/aeve.tar'
$aldrusImage = './out/images/aldrus.tar'
$azureHost = ''

& docker build . -t aeve -f .\Dockerfile-aeve
& docker build . -t aldrus -f .\Dockerfile-aldrus
& docker save -o $aeveImage aeve
& docker save -o $aldrusImage aldrus
& pscp -P 443 -i .\azure\docker-test.ppk $aeveImage $aldrusImage "$azureHost`:/home/docker-test/images/"
& pscp -P 443 -i .\azure\docker-test.ppk .\docker-compose.yml "$azureHost`:/home/docker-test/tmp/"
