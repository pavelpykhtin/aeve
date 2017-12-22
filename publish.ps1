$aeveImage = './out/images/aeve.tar'
$aldrusImage = './out/images/aldrus.tar'
$yvratImage = './out/images/yvrat.tar'
$azureHost = 'docker-test@docker-test-107h8q8b.cloudapp.net'

& docker build . -t aeve -f .\Dockerfile-aeve
& docker build . -t aldrus -f .\Dockerfile-aldrus
& docker build . -t yvrat -f .\Dockerfile-yvrat
& docker save -o $aeveImage aeve
& docker save -o $aldrusImage aldrus
& docker save -o $yvratImage yvrat
& pscp -P 443 -i .\azure\docker-test.ppk $aeveImage $aldrusImage $yvratImage "$azureHost`:/home/docker-test/images/"
& pscp -P 443 -i .\azure\docker-test.ppk .\docker-compose.yml "$azureHost`:/home/docker-test/tmp/"
