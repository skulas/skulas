<template>
    <div id="products">
        <h1 v-bind:title='message'>Products</h1>
        <h2>{{ products[0] }} are in stock.</h2>
        <div>
            <ul>
                <li v-for="product in products">
                    {{ product }}
                </li>
            </ul>
        </div>
        <div>
            <p>If you hover over <b>Products</b> the binded message will be displayed.</p>
        </div>
    </div>
</template>

<script>
import axios from 'axios'

export default {
  name: 'products',
  data () {
    return {
      products: ['Boots', 'Jacket', 'Pants', 'Socks'],
      message: 'Products we have in Stock'
    }
  },
  methods: {
    getProductsFromBackend () {
      const path = 'http://localhost:5555/api/getProducts'
      axios
        .get(path)
        .then(response => {
          console.log('Got response from backend')
          console.log('response: ' + JSON.stringify(response))
          console.log('Products:' + response.data.products)
          if (response.data.products !== undefined) {
            this.products = response.data.products
          }
        })
        .catch(error => {
          console.log(error)
        })
    }
  },
  created () {
    console.log('Created')
    this.getProductsFromBackend()
  }
}
</script>
