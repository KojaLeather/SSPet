<template>
  <div id="app">
      <div class="container">
          <h1>Start Stream on ID</h1>
          <form @submit.prevent="startStream">
              <button type="submit">Start Stream</button>
          </form>
          <p v-if="response">{{ response }}</p>
      </div>
      <div class="container">
          <h1>Stop Stream on ID</h1>
          <form @submit.prevent="stopStream">
              <button type="submit">Stop Stream</button>
          </form>
      </div>
      <div class="container">
          <button @click="getManifestFile">Load Video</button>
          <video ref="videoPlayer" controls preload="auto" width="640" height="360">
          </video>
      </div>
  </div>
</template>

<script>
import axios from 'axios'
import Hls from 'hls.js';

export default {
  data() {
    return {
      id: null, 
      response: null, 
      baseUrl: 'https://localhost:7239/api'
    }
  },
  methods: {
    async startStream() {
      try {
        const response = await axios.get(`${this.baseUrl}/Stream/start`);
        this.response = response.data;
      } catch (error) {
        console.error('Error starting the stream:', error);
        this.response = 'Failed to start stream';
      }
    },
    
    async stopStream() {
      try {
        const response = await axios.get(`${this.baseUrl}/Stream/stop`);
        this.response = response.data;
      } catch (error) {
        console.error('Error stopping the stream:', error);
        this.response = 'Failed to stop stream';
      }
    },

    getManifestFile() {
      const video = this.$refs.videoPlayer; 
      const videoSrc = `${this.baseUrl}/LoadingFiles/GetStreamFiles?fileName=output.m3u8`;

      if (Hls.isSupported()) {
        const hls = new Hls();
        hls.loadSource(videoSrc);
        hls.attachMedia(video);
        hls.on(Hls.Events.MANIFEST_PARSED, () => {
          video.play();
        });
      } else if (video.canPlayType('application/vnd.apple.mpegurl')) {
        video.src = videoSrc;
        video.addEventListener('loadedmetadata', () => {
          video.play();
        });
      }
    }
  }
}
</script>

<style scoped>
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
