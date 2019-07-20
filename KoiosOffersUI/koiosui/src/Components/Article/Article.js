import * as ReactStrap from 'reactstrap';
import Autosuggest from 'react-autosuggest';
import axios from 'axios';
import './Article.css';

const Article = () => {
    // <div className="article">
    //     <div className="articleName">

    //     </div>

    //     <div className="articlePrice">

    //     </div>

    //     <div className="selectArticle">

    //     </div>
    // </div>
};

export const saveArticle = async (article) => {
    let result = {
        data: "",
        status: 0
    }
    await axios.post('http://localhost:59189/api/Article/Add', {
        "name": article.name,
        "unitPrice": article.unitPrice
    }).then(response => {
        return response.data;
    }).catch(error => {
        return -1;
    });
}

export const getSuggestionsByName = async (name, skip, take) => {
    let result;
    await axios.get('http://localhost:59189/api/Article/Paginated?' +
     'name=' + name + 
     '&skip=' + skip +
     '&take=' + take)
        .then(response => {
            result = response.data;
        }).catch(error => {
            result = error;
        });

    return result;
};

export const deleteArticle = async (id) => {
    await axios.delete('http://localhost:59189/api/Article/Delete?id=' + id)
        .then(response => {
            return {status: response.status};
        }).catch(error => {
            return {status: 400};
        });
}

export const updateArticle = async (article) => {
    let result;
    console.log("Zadani id: " + article.id);
    console.log("Zadani name: " + article.name);
    console.log("Zadani unitPrice: " + article.unitPrice);
    await axios.put('http://localhost:59189/api/Article/Update', {
        "id": article.id,
        "name": article.name,
        "unitPrice": article.unitPrice
    }).then(response => {
        return {
            status: response.status
        }
    }).catch(error => {
        console.log("Nekakav error: " + error);
            return {
                status: 400
            }
    });

    return result;
}

export const getAllArticles = async (article) => {
    let result;
    await axios.put('http://localhost:59189/api/Article/Update', {
        "id": article.id,
        "name": article.name,
        "unitPrice": article.unitPrice
    }).then(response => {
            return {
                status: response.status
            }
    }).catch(error => {
            return {
                status: 400
            }
    });

    return result;
}

export default Article;