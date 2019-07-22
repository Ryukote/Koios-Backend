import React, { createContext } from 'react';
import { Button } from 'reactstrap';
import axios from 'axios';
import lodash from 'lodash';
import './OfferContext.css';

export const OfferContext = createContext({
  articleCollection: [],
  addToCollection: () => [],
  removeFromCollection: () => [],
  renderArticle(value, key) {},
  returnCollection: () => {},
  createOffer: () => {},
  getData: () => {},
  suggestions: []
});

export class OfferProvider extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            articleCollection: [],
            getOffer: this.getOffer.bind(this),
            addToCollection: this.addToCollection.bind(this),
            returnCollection: this.returnCollection.bind(this),
            renderArticle: this.renderArticle.bind(this),
            createOffer: this.createOffer.bind(this),
            getData: this.getData.bind(this),
            suggestions: [],
            offerId: 0,
            offer: {
                Id: 0,
                Number: 0,
                CreatedAt: "",
                TotalPrice: 0
            },
            toggle: true
        }

        this.getOffer = this.getOffer.bind(this);
        this.addToCollection = this.addToCollection.bind(this);
        this.removeFromCollection = this.removeFromCollection.bind(this);
        this.returnCollection = this.returnCollection.bind(this);
        this.renderArticle = this.renderArticle.bind(this);
        this.createOffer = this.createOffer.bind(this);
        this.getData = this.getData.bind(this);
    }

    createOffer = () => {
        if(this.state.toggle) {
            this.setState({
                toggle: false
            })
        }
        else {
            this.setState({
                toggle: true
            })
        }

        return this.state.toggle;
    }

    getOffer = async (offerId) => {
        this.setState({
            articleCollection: []
        })

        let url = "http://localhost:59189/api/Offer/GetOfferArticles?offerId="
            + offerId;

        let offerUrl = "http://localhost:59189/api/Offer/GetById?id="
            + offerId;

        await axios.get(offerUrl).then(response => {
            console.log(response.data);
            this.setState({
                offer: response.data
            });
        }).catch(error => {
            alert("Offer with given id doesn't exist.");
            this.setState({
                articleCollection: []
            })
            throw error;
        })

        await axios.get(url).then(response => {
            this.setState({
                articleCollection: response.data
            });

            this.setState({
                offerId: offerId
            });
        }).catch(error => {
            this.setState({
                articleCollection: []
            })

            throw error;
        })
    }    
    
    addToCollection = (article, offerId) => {
        let tmpArray;

        this.state.articleCollection != []
            ? tmpArray = this.state.articleCollection
            : tmpArray = []

        tmpArray.push(article);

        this.setState({
            articleCollection: tmpArray
        })

        this.addOfferArticle(offerId, article.id);
    }

    addOfferArticle = async (offerId, articleId) => {
        console.log("Pizda");
        console.log(offerId);
        console.log(articleId);
        await axios.post('http://localhost:59189/api/Offer/AddOfferArticle', {
            "OfferId": offerId,
            "ArticleId": articleId
        }).then(response => {
                return response.data;
            }).catch(() => {
                return -1;
            });
    }

    deleteArticleFromOffer = async (articleId) => {
        let url = "http://localhost:59189/api/Offer/DeleteOfferArticle?offerId="
            + this.state.offerId
            + "&articleId="
            + articleId;

        await axios.delete(url)
            .then(() => {
                alert("Article removed from offer");
            }).catch(error => {
                alert("Something went wrong");
                throw error;
            })
    }

    removeFromCollection = (article) => {
        let tmpArray = this.state.articleCollection;

        this.deleteArticleFromOffer(article.id)
            .then(() => {
                tmpArray = lodash.remove(tmpArray, value => 
                    value.Id !== article.id
                )
        
            this.setState({
                articleCollection: tmpArray
            })
        })
    }

    returnCollection = () => {
        return this.state.articleCollection;
    }

    renderArticle = (value, key) => {
        if(value != null){
            return(
                <div className="articlePosition" key={key}>
                    <div>
                        {"Article id: " + value.id}
                    </div>
    
                    <div>
                        {"Article name: " + value.name}
                    </div>
    
                    <div>
                        {"Article price: " + value.unitPrice}
                    </div>
    
                    <div>
                        <Button
                            color="danger"
                            onClick={() => this.removeFromCollection({...value})}
                        >
                            Remove from offer
                        </Button>
                    </div>
                </div>
            );
        }
        else{
            return(
                <div></div>
            );
        }
    }

    getData = async (searchText) => {
        let result;

        let url = 'http://localhost:59189/api/Article/Paginated?name='
            + searchText
            + '&skip=0&take=10';

        if(searchText != null) {
            await axios.get(url)
                .then(response => {
                    result = response.data
                    return result;
                }).catch(error => {
                    alert("Something went wrong");
                    throw error;
                });

            this.setState({
                suggestions: result
            });
        }
    }

    render() {
        return (
            <OfferContext.Provider value={{...this.state}}>
                {{...this.props.children}}
            </OfferContext.Provider>
        );
    }
}

export const OfferConsumer = OfferContext.Consumer;