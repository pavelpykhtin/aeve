class FuckCors {
    get(url) {
        return new Promise((resolve, reject) => {
            const iframe = document.createElement('iframe');
            iframe.src = url;

            iframe.addEventListener('load', () => {
                console.log('loaded');
                resolve(iframe.contentDocument.body.innerText);
                iframe.remove();
            });

            document.body.appendChild(iframe);
        });
    }
}

export default new FuckCors();