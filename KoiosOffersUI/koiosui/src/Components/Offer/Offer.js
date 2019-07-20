import axios from 'axios';

export const deleteOfferArticle = async (offerId, articleId) => {
    await axios.delete('http://localhost:59189/api/Offer/DeleteOfferArticle?offerId=' + offerId +
        '&&articleId=' + articleId)
        .then(response => {
            return response.data;
        }).catch(error => {
            return -1;
        });
}

export const addOfferArticle = async (offerId, articleId) => {
    await axios.post('http://localhost:59189/api/Offer/AddOfferArticle', {
        "offerId": offerId,
        "articleId": articleId
    }).then(response => {
            return response.data;
        }).catch(error => {
            return -1;
        });
}

export const getOfferById = async (offerId) => {
    await axios.get('http://localhost:59189/api/Offer/GetOfferArticles?offerId=' + offerId)
        .then(response => {
            return response.data;
        }).catch(error => {
            return -1;
        });
}

export const getOfferArticles = async (offerId) => {
    await axios.get('http://localhost:59189/api/Offer/GetOfferArticles?offerId=' + offerId)
        .then(response => {
            return response.data;
        }).catch(error => {
            return -1;
        });
}

export const getPaginated = async (offerNumber) => {
    console.log(offerNumber);
    let url = 'http://localhost:59189/api/Offer/GetPaginated?offerNumber=' + offerNumber
    + '&skip=0&take=10';
    console.log(url);
    await axios.get(url)
        .then(response => {
            return response.data;
        }).catch(error => {
            console.log(error);
            return -1;
        });
}
